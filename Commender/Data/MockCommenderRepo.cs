using Commender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commender.Data
{
    public class MockCommenderRepo : ICommenderRepo
    {
        public void CreateCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command command)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>
            {
                new Command { Id = 0, HowTo = "Boil egg", Line = "Boil water", Platform = "Kettle and pan" },
                new Command { Id = 0, HowTo = "Boil egg", Line = "Boil water", Platform = "Kettle and pan" }

        };
            return commands;
    }


        public Command GetCommandById(int id)
        {
            return new Command { Id = 0, HowTo = "Boil egg", Line = "Boil water", Platform = "Kettle and pan" };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
