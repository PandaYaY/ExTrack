package config

import (
	"time"

	"github.com/ilyakaznacheev/cleanenv"
)

type Config struct {
	Port           string               `yaml:"port" env:"PORT" env-default:"8080"`
	ProverkaChecka ProverkaCheckaConfig `yaml:"proverkaChecka" env-required:"true"`
}

type ProverkaCheckaConfig struct {
	BaseURL        string        `yaml:"baseUrl" env-required:"true"`
	Token          string        `yaml:"token" env-required:"true"`
	TimeoutSeconds time.Duration `yaml:"timeoutSeconds" env-default:"30"`
}

var cfg *Config

func MustLoad() (*Config, error) {
	if cfg != nil {
		return cfg, nil
	}

	cfg = &Config{}
	err := cleanenv.ReadConfig("config.yml", cfg)
	if err != nil {
		return nil, err
	}

	return cfg, nil
}
