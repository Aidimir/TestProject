using System;

namespace Dal.Interfaces
{
	public interface IPublicGame
	{
        public int Id { get; set; }

        public string Title { get; set; }

        public string Developer { get; set; }

        public List<string> Genres { get; set; }
    }
}

