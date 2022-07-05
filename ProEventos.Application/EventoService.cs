﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;
        

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }


        public async Task<EventoDto> addEventos(EventoDto model)
        {
            try
            {
                var evento  = _mapper.Map<Evento>(model);
                _geralPersist.add(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var result = await _eventoPersist.GetEventosByIdASync(evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdASync(eventoId,false);
                if (evento == null)
                    return false;
                _geralPersist.Delete(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
               
                throw new Exception(message:e.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventosByIdASync(eventoId, false);
                if (evento == null) 
                    return null;
            
                model.Id = evento.Id;
                _mapper.Map(model,evento);
                _geralPersist.Update(evento);
                if (await _geralPersist.SaveChangesAsync())
                {
                    var result  = await _eventoPersist.GetEventosByIdASync(evento.Id, false);
                    return _mapper.Map<EventoDto>(result);
                }

                return null;
            }
            catch (Exception e)
            {

                throw new Exception(message: e.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrante = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrante);
                if(eventos == null) return null;
                
                return _mapper.Map<EventoDto[]>(eventos);
            }
            catch (Exception e)
            {
                
                throw new Exception(message:e.Message);
            }
        }

        public async  Task<EventoDto[]> GetAllEventosbyTemaAsync(string tema, bool includePalestrante = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaASync(tema, includePalestrante);
                if(eventos == null) return null;
                
                return _mapper.Map<EventoDto[]>(eventos);

            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }

        public async Task<EventoDto> GetEventosByIdAsync(int id, bool includePalestrante = false)
        {
            try
            {
                var evento =  await _eventoPersist.GetEventosByIdASync(id, includePalestrante);
                if(evento == null) return null;

                return  _mapper.Map<EventoDto>(evento);
            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }
    }
}