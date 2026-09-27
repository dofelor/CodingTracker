using CodingTracker.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Controllers
{
    internal class CodingSessionsController : BaseController, ICodingSessionsController
    {
        private readonly DbContext _context;

        public CodingSessionsController(DbContext context)
        {
            _context = context;
        }

        public void ViewSessions()
        {
            using(var connection = _context.CreateConnection())
            {
                List<CodingSession> sessions = connection.Query<CodingSession>("SELECT * FROM CodingSession").ToList();
                if (sessions.Any())
                {
                    DisplayTable(sessions, "ALL OF THEM");
                }
                else
                {
                    DisplayMessage("Sessions was not found :(", color: "blue");
                }
            }
        }

        public CodingSession? GetCodingSessionById(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = "SELECT * FROM CodingSession WHERE Id = @Id";
                CodingSession? session = connection.QuerySingleOrDefault<CodingSession>(sql, new {Id = id});
                return session;
            }
            
        }

        public void AddSession()
        {
            using(var connection = _context.CreateConnection())
            {
                var dateTimeUserInput = DateTimeInput();

                string sql = "INSERT INTO CodingSession (StartTime, EndTime) VALUES (@StartTime, @EndTime)";


                var newSession = new CodingSession { StartTime = dateTimeUserInput.startTime, EndTime = dateTimeUserInput.endTime};
                var rows = connection.Execute(sql, newSession);
                DisplayMessage("Session was added.");
            }
        }

        public void DeleteSession() 
        {
            ViewSessions();
            using(var connection = _context.CreateConnection())
            {
                int userIdInput = InputId();
                string sql = "DELETE FROM CodingSession WHERE Id = @Id";
                int rows = connection.Execute(sql, new {Id = userIdInput});
                if (rows == 0)
                {
                    DisplayError($"The session with that ID - {userIdInput} does not exist.");
                }
                else
                {
                    DisplayMessage($"The session with this ID - {userIdInput} was deleted.");
                }
            }
        }

        public void UpdateSession() 
        {
            ViewSessions();
            using(var connection = _context.CreateConnection())
            {
                int userIdInput = InputId();
                var session = GetCodingSessionById(userIdInput);

                //Since Dapper calls the `Execute` method—which returns the number of affected rows—and the check can be
                //performed based on that count, a null check might be redundant;
                //however, it could be convenient for the user (saving them from having to enter new dates), so I’ll keep it.

                if (session is null)
                {
                    DisplayError("No session with this ID was found");
                    return;
                }

                (DateTime updatedStartTime, DateTime updatedEndTime) = DateTimeUpdateInput(session);
                string sql = "UPDATE CodingSession SET StartTime = @StartTime, EndTime = @EndTime WHERE Id = @Id";

                int rows = connection.Execute(sql, new
                {
                    StartTime = updatedStartTime,
                    EndTime = updatedEndTime,
                    Id = session.Id
                });

                //Checking the number of modified rows would also be redundant, but I'll leave it in.
                if (rows == 0)
                {
                    DisplayError($"The session with that ID - {userIdInput} does not exist.");
                }
                else
                {
                    DisplayMessage($"The session with this ID - {userIdInput} was updated.");
                }


            }
        }
    }
}
