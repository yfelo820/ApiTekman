using System;
using System.ComponentModel.DataAnnotations;
using Api.Entities;

namespace Api.Entities.Content
{
	public class ItemStatic : Item
	{
        public override ItemType Type => ItemType.Static;
    }
}