﻿using ReservationService.Events;
using ReservationService.Messaging.Interface;

namespace ReservationService.Service.Background;

public class PaymentConsumerService : BackgroundService
{
    private readonly IConsumer<PaymentEvent> _consumer;
    
    public PaymentConsumerService(IConsumer<PaymentEvent> consumer)
    {
        _consumer = consumer;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            // Chỉ gọi consume một lần khi service khởi chạy
            _consumer.Consume(onMessage: async (message) =>
            {
                // In message ra console để kiểm tra
                Console.WriteLine($"Received PaymentId: {message.PaymentId}");
                await Task.CompletedTask;
            });
            
            // Vòng lặp này chỉ để giữ cho background service tiếp tục chạy
            while (!stoppingToken.IsCancellationRequested)
            {
                // Chờ 1 giây để tránh quá tải CPU
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}