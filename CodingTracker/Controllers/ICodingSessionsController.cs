using CodingTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Controllers
{
    internal interface ICodingSessionsController
    {
        void ViewSessions();
        CodingSession? GetCodingSessionById(int id);
        void AddSession();
        void DeleteSession();
        void UpdateSession();
    }
}
