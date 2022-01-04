using AutoMapper;

using BusinessLayer.Commands.Command.Teams;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Commands.Handler.Teams
{
    public class TeamCommandHandler : 
        Interfaces.ICommandHandler<RenameTeamCommand, CommandResponse>, 
        Interfaces.ICommandHandler<DeleteTeamCommand, CommandResponse>,
        Interfaces.ICommandHandler<CreateTeamCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable
    {
        // FIELSD
        IUnitOfWork unitOfWork;       

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // METHODS
        public CommandResponse Execute(RenameTeamCommand command)
        {
            // get data
            Team teamToUpdate = unitOfWork.GetRepository<Team, TeamRepository>().Get(command.RenameTeamDTO.Id);

            // check
            if (teamToUpdate == null)
            {
                return new CommandResponse()
                {
                    IsSucessed = false,
                    Message = "Wrong Id"
                };
            }

            // update

            bool isUpdated = false;
            string message = "Updated";

            try
            {
                teamToUpdate.Name = command.RenameTeamDTO.Name;
                isUpdated = true;

                unitOfWork.Update(teamToUpdate);
                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                isUpdated = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // resulting
            return new CommandResponse()
            {
                IsSucessed = isUpdated,
                Message = message
            };
        }

        public CommandResponse Execute(DeleteTeamCommand command)
        {
            // delete team
            bool isDeleted = false;
            string message = "Team deleated";
            try
            {
                isDeleted = unitOfWork.GetRepository<Team, TeamRepository>().Delete(command.TeamId);
                if (!isDeleted) message = "Could not delete team";
                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                isDeleted = false;
                message = Common.Algorithms.GetFullText(e);

            }

            // result
            return new CommandResponse
            {
                IsSucessed = isDeleted,
                Message = message
            };
        }

        public CommandResponse Execute(CreateTeamCommand command)
        {
            // map team
            TeamRepository teamRepository = unitOfWork.GetRepository<Team, TeamRepository>();
            Team team = command.ServiceProvider.GetService<IMapper>()
                        .Map<Team>(command.CreateTeamDTO);

            // insert
            bool insertingStatus = false;
            string message = "Team created";
            try
            {
                insertingStatus = teamRepository.Insert(team);
                unitOfWork.Save();
            }
            catch (System.Exception e)
            {
                insertingStatus = false;
                message = Common.Algorithms.GetFullText(e);
            }

            // result
            return new CommandResponse
            {
                IsSucessed = insertingStatus,
                Message = message
            };
        }
    }
}
