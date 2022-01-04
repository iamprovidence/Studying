using AutoMapper;

using BusinessLayer.Commands.Command.Teams;

using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using System.Threading.Tasks;

namespace BusinessLayer.Commands.Handler.Teams
{
    public class TeamCommandHandler : 
        Interfaces.ICommandHandler<UpdateTeamCommand, CommandResponse>, 
        Interfaces.ICommandHandler<DeleteTeamCommand, CommandResponse>,
        Interfaces.ICommandHandler<CreateTeamCommand, CommandResponse>,
        Interfaces.IUnitOfWorkSettable, Interfaces.IMapperSettable
    {
        // FIELSD
        IMapper mapper;
        IUnitOfWork unitOfWork;

        // CONSTRUCTORS
        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        // METHODS
        public async Task<CommandResponse> ExecuteAsync(UpdateTeamCommand command)
        {
            // get data
            Team teamToUpdate = await unitOfWork.GetRepository<Team, TeamRepository>().GetAsync(command.UpdateTeamDTO.Id);

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
                teamToUpdate.Name = command.UpdateTeamDTO.Name;
                isUpdated = true;

                unitOfWork.Update(teamToUpdate);
                await unitOfWork.SaveAsync();
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

        public async Task<CommandResponse> ExecuteAsync(DeleteTeamCommand command)
        {
            // delete team
            bool isDeleted = false;
            string message = "Team deleated";
            try
            {
                isDeleted = await unitOfWork.GetRepository<Team, TeamRepository>().DeleteAsync(command.TeamId);
                if (!isDeleted) message = "Could not delete team";
                await unitOfWork.SaveAsync();
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

        public async Task<CommandResponse> ExecuteAsync(CreateTeamCommand command)
        {
            // map team
            TeamRepository teamRepository = unitOfWork.GetRepository<Team, TeamRepository>();
            Team team = mapper.Map<Team>(command.CreateTeamDTO);

            // insert
            bool insertingStatus = false;
            string message = "Team created";
            try
            {
                insertingStatus = await teamRepository.InsertAsync(team);
                await unitOfWork.SaveAsync();
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
