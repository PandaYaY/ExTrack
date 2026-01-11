package proverkacheka

import (
	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"time"

	"extrack/internal/config"
	"extrack/internal/proverkacheka/models"
)

type Client struct {
	baseURL    string
	httpClient *http.Client
	token      string
	userAgent  string
}

type ClientOption func(*Client)

const defaultUserAgent = "extrack/1.0"

func NewClient() *Client {
	cfg, err := config.MustLoad()
	if err != nil {
		log.Panicf("Error loading config: %v", err)
	}

	timeout := cfg.ProverkaChecka.TimeoutSeconds * time.Second
	client := &Client{
		baseURL: cfg.ProverkaChecka.BaseURL,
		httpClient: &http.Client{
			Timeout: timeout,
		},
		token:     cfg.ProverkaChecka.Token,
		userAgent: defaultUserAgent,
	}

	return client
}

func (c *Client) doRequest(ctx context.Context, method, endpoint string, body interface{}) (*http.Response, error) {
	url := c.baseURL + endpoint

	var reqBody io.Reader
	if body != nil {
		jsonData, err := json.Marshal(body)
		if err != nil {
			return nil, err
		}
		reqBody = bytes.NewBuffer(jsonData)
	}

	req, err := http.NewRequestWithContext(ctx, method, url, reqBody)
	if err != nil {
		return nil, err
	}

	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Accept", "application/json")
	req.Header.Set("User-Agent", c.userAgent)

	resp, err := c.httpClient.Do(req)
	if err != nil {
		return nil, err
	}

	return resp, nil
}

func (c *Client) parseResponse(resp *http.Response, result interface{}) error {
	defer resp.Body.Close()

	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return err
	}

	if resp.StatusCode < http.StatusOK || resp.StatusCode >= http.StatusMultipleChoices {
		return fmt.Errorf("API error: status %d, body: %s", resp.StatusCode, string(body))
	}

	if err := json.Unmarshal(body, result); err != nil {
		return err
	}

	return nil
}

func (c *Client) CheckInfo(ctx context.Context, request *models.CheckInfoRequest) (*models.CheckInfoResponse, error) {
	resp, err := c.doRequest(ctx, http.MethodPost, "/api/v1/check/get", request)
	if err != nil {
		return nil, err
	}

	var result models.CheckInfoResponse
	if err := c.parseResponse(resp, &result); err != nil {
		return nil, err
	}

	return &result, nil
}
