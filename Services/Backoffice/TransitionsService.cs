using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Api.Entities.Content;

namespace Api.Services.Backoffice
{
	public class TransitionsService : IApiService<Transition>
	{
		private readonly float maxScale = 2.0f;
		private readonly float maxBlur = 10.0f;
		private readonly float maxOpacity = 1.0f;
		private readonly float maxGrayscale = 100.0f;
		private readonly float maxBrightness = 200.0f;

		private readonly IContentRepository<Transition> _transitions;
		private readonly IContentRepository<TransitionProperty> _transitionProps;
		public TransitionsService(
			IContentRepository<Transition> transitions,
			IContentRepository<TransitionProperty> transitionProps
		)
		{
			_transitions = transitions;
			_transitionProps = transitionProps;
		}

		public async Task<List<Transition>> GetAll()
		{
			return (await _transitions.GetAll()).OrderBy(c => c.Name).ToList();
		}

		public async Task<Transition> GetSingle(Guid id)
		{
			return await _transitions
				.Query(new[] { "TransitionProperties" })
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<Transition> Add(Transition transition)
		{
			CheckValues(transition);
			foreach (TransitionProperty tProp in transition.TransitionProperties)
			{
				tProp.TransitionId = transition.Id;
			}
			return await _transitions.Add(transition);
		}

		public async Task<Transition> Update(Transition transition)
		{
			CheckValues(transition);
			foreach (TransitionProperty tProp in transition.TransitionProperties)
			{
				tProp.Id = Guid.Empty;
				tProp.TransitionId = transition.Id;
			}
			// Deleting old transition properties
			await _transitionProps.Delete(
				await _transitionProps.Find(t => t.TransitionId == transition.Id)
			);
			// adding transition properties
			await _transitionProps.Add(transition.TransitionProperties);
			// Updating transition
			await _transitions.Update(transition);
			return transition;
		}

		public async Task<Transition> Delete(Guid id)
		{
			Transition transition = await GetSingle(id);
			await _transitions.Delete(transition);
			return transition;
		}

		private void CheckValues(Transition t)
		{
			// Check min and max values
			foreach (TransitionProperty tp in t.TransitionProperties)
			{
				if (tp.Property == PropertyType.Shadow
					|| tp.Property == PropertyType.Background
					|| tp.Value == ""
				) continue;
				float value;
				try
				{
					value = float.Parse(tp.Value, CultureInfo.InvariantCulture.NumberFormat);
				}
				catch (Exception)
				{
					// if the value cannot be parsed, we will remove the value prop. 
					// This will delete the transitionProp
					tp.Value = "";
					continue;
				}
				float maxValue = maxGrayscale;
				switch (tp.Property)
				{
					case PropertyType.Scale:
						maxValue = maxScale;
						break;
					case PropertyType.Blur:
						maxValue = maxBlur;
						break;
					case PropertyType.Opacity:
						maxValue = maxOpacity;
						break;
					case PropertyType.Brightness:
						maxValue = maxBrightness;
						break;
				}
				value = Math.Min(Math.Max(0, value), maxValue);
				tp.Value = value.ToString(CultureInfo.InvariantCulture);
			}
			// Remove empty values
			t.TransitionProperties = t.TransitionProperties.Except(
				t.TransitionProperties.Where(tp => tp.Value == "").ToList()
			).ToList();
		}
	}
}