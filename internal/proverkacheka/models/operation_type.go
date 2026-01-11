package models

type OperationType int

const (
	OperationTypePrihod         OperationType = 1
	OperationTypeVozvratPrihoda OperationType = 2
	OperationTypeRashod         OperationType = 3
	OperationTypeVozvratRashoda OperationType = 4
)
