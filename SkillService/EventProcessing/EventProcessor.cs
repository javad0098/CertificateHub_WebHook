using System;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SkillService.Data;
using SkillService.Dtos;
using SkillService.Models;

namespace SkillService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CertificatePublished:
                    AddCertificate(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);
            switch (eventType.Event)
            {
                case "Certificate_Published":
                    Console.WriteLine("--> Certificate Published Event Detected");
                    return EventType.CertificatePublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async void AddCertificate(string certificatePublishedDtoMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ISkillRepo>();

                var certificatePublishedDto = JsonSerializer.Deserialize<CertificatePublishedDto>(certificatePublishedDtoMessage);

                try
                {
                    var cer = _mapper.Map<Certificate>(certificatePublishedDto);
                    if (!await repo.ExternalCertificateExistsAsync(cer.ExternalID))
                    {
                        await repo.CreateCertificateAsync(cer);
                        await repo.SaveChangesAsync();
                        Console.WriteLine("--> Certificate added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Certificate already exisits...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Certificate to DB {ex.Message}");
                }
            }
        }
    }

    internal enum EventType
    {
        CertificatePublished,
        Undetermined,
    }
}
