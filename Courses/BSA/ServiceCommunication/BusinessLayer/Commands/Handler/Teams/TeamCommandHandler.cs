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
            teamToUpdate.Name = command.RenameTeamDTO.Name;
            return new CommandResponse()
            {
                IsSucessed = true,
                Message = "Updated"
            };
        }

        public CommandResponse Execute(DeleteTeamCommand command)
        {
            // delete team
            if (unitOfWork.GetRepository<Team, TeamRepository>().Delete(command.TeamId))
            {
                // update foreign keys
                foreach (User user in unitOfWork.GetRepository<User, UserRepository>().Get())
                {
                    user.TeamId = null;
                }
                foreach (Project project in unitOfWork.GetRepository<Project, ProjectRepository>().Get())
                {
                    project.TeamId = null;
                }

                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = "Deleted"
                };
            }
            else return new CommandResponse
            {
                IsSucessed = false,
                Message = "Could not delete. Check id"
            };
        }

        public CommandResponse Execute(CreateTeamCommand command)
        {
            // map team
            TeamRepository teamRepository = unitOfWork.GetRepository<Team, TeamRepository>();
            Team team = command.ServiceProvider.GetService<IMapper>()
                        .Map<Team>(command.CreateTeamDTO);
            team.Id = teamRepository.MaxId() + 1;

            // insert
            if (teamRepository.Insert(team))
            {
                return new CommandResponse
                {
                    IsSucessed = true,
                    Message = $"New team added. Team id = {team.Id}"
                };
            }
            else
            {
                return new CommandResponse
                {
                    IsSucessed = false,
                    Message = "Could not add new team",
                };
            }
        }
    }
}
