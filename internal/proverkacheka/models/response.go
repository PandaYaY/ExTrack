package models

type ScanResultEnum int

const (
	ScanResultEnumInCorrect       ScanResultEnum = 0
	ScanResultEnumSuccess         ScanResultEnum = 1
	ScanResultEnumNotSuccessYet   ScanResultEnum = 2
	ScanResultEnumTooManyRequests ScanResultEnum = 3
	ScanResultEnumPleaseWait      ScanResultEnum = 4
	ScanResultEnumAnother         ScanResultEnum = 5
)

type Item struct {
	Quantity float64 `json:"quantity"`
	Price    int64   `json:"price"`
	Sum      int64   `json:"sum"`
	Nds      int64   `json:"nds"`
	NdsSum   int64   `json:"ndsSum"`
	Name     string  `json:"name"`
}

type CheckInfoDataJson struct {
	User               string `json:"user"`
	RetailPlace        string `json:"retailPlace"`
	RetailPlaceAddress string `json:"retailPlaceAddress"`
	Items              []Item `json:"items"`
	Nds10              int64  `json:"nds10"`
	Nds18              int64  `json:"nds18"`
	FnsUrl             string `json:"fnsUrl"`
	Timestamp          string `json:"dateTime"`
	Sum                int64  `json:"totalSum"`
}

type CheckInfoData struct {
	JsonData CheckInfoDataJson `json:"json"`
	HtmlData string            `json:"html"`
}

type RequestData struct {
	Fn            string        `json:"fn"`
	Fd            string        `json:"fd"`
	Fp            string        `json:"fp"`
	Timestamp     string        `json:"check_time"`
	OperationType OperationType `json:"type"`
	Sum           string        `json:"sum"`
}

type RequestInfo struct {
	QrUrl      string      `json:"qrurl"`
	QrFileInfo string      `json:"qrfile"`
	QrRaw      string      `json:"qrraw"`
	Manual     RequestData `json:"manual"`
}

type CheckInfoResponse struct {
	StatusCode  ScanResultEnum `json:"code"`
	IsFirst     byte           `json:"first"`
	Data        CheckInfoData  `json:"data"`
	RequestInfo RequestInfo    `json:"request"`
}
