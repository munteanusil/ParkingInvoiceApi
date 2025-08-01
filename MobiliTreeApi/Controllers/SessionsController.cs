﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Repositories;

namespace MobiliTreeApi.Controllers
{
    [ApiController]
    [Route("sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsRepository _sessionsRepository;

        public SessionsController(ISessionsRepository sessionsRepository)
        {
            _sessionsRepository = sessionsRepository;
        }

        [HttpPost]
        public ActionResult Post(Session value)
        {
            _sessionsRepository.AddSession(value);
            return Ok();
        }

        [HttpGet("{parkingFacilityId}")]
        public List<Session> Get(string parkingFacilityId)
        {
            return _sessionsRepository.GetSessions(parkingFacilityId);
        }
    }
}
