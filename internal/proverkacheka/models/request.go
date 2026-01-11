package models

type CheckInfoRequest struct {
	Token                     string        `json:"token"`
	FiscalStorageDeviceNumber int64         `json:"fn"`
	FiscalDocumentNumber      int64         `json:"fd"`
	DocumentFiscalAttribute   int64         `json:"fp"`
	Timestamp                 string        `json:"t"`
	OperationType             OperationType `json:"n"`
	Sum                       float64       `json:"s"`
	IsQr                      bool          `json:"qr"`
}
