using PaleTree.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaleTree.Plugins.EntityLogger
{
	public interface IEntity
	{
		int Handle { get; }
		int Hp { get; }
		//Map Map { get; }
		Position Position { get; }
		NpcType NpcType { get; }
		int Id { get; }
		string Name { get; }
		string GetInfo();
		string GetScript();
		List<IEntity> Entities { get; }
	}
}
