using AppService.Interfaces;

namespace AppService.Commands
{
    public class AddLocationDescriptionCommand : ICommand

    {
        public int LocationId { get; }

        public string Description { get; }

        public AddLocationDescriptionCommand( int locationId, string description)
        {
            this.Description = description;
            this.LocationId = locationId;
        }
    }
}