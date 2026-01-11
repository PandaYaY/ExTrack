package main

import (
	"log"

	"github.com/gin-gonic/gin"

	"extrack/internal/config"
)

func main() {
	cfg, err := config.MustLoad()
	if err != nil {
		log.Panicf("Error loading config: %v", err)
	}

	log.Println(cfg.Port)

	r := gin.Default()
	r.GET("/ping", func(c *gin.Context) {
		c.JSON(200, gin.H{
			"message": "pong",
		})
	})
	r.POST("/api/v1/check", func(c *gin.Context) {

	})
	r.Run(":" + cfg.Port)
}
