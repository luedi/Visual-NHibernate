﻿using System.Linq;
using System.Text;
using ArchAngel.Providers.CodeProvider.DotNet;
using log4net;

namespace ArchAngel.Providers.CodeProvider.CodeInsertions
{
	public class RemoveFieldFromClassAction : CodeInsertionAction
	{
		static readonly char[] WhitespaceChars = new char[] { ' ', '\t', '\r', '\n' };
		private static readonly ILog log = LogManager.GetLogger(typeof(RemoveFieldFromClassAction));
		public Field FieldToDelete { get; set; }

		public RemoveFieldFromClassAction(Field fieldToDelete)
		{
			FieldToDelete = fieldToDelete;
		}

		public override ActionResult ApplyActionTo(StringBuilder sb)
		{
			// Search PropertyToChange TextRange for name
			int searchStart = FieldToDelete.TextRange.StartOffset;
			int searchEnd = FieldToDelete.TextRange.EndOffset;

			// Also remove all characters up to next line break
			for (int i = searchEnd; i < sb.Length - 1; i++)
			{
				char character = sb[i];

				if (!WhitespaceChars.Contains(character))
					break;

				if (character == '\r')
				{
					if (i + 1 < sb.Length &&
						sb[i + 1] == '\n')
					{
						searchEnd = i + 2;
						break;
					}
					else
					{
						searchEnd = i + 1;
						break;
					}
				}
				else if (character == '\n')
				{
					searchEnd = i + 1;
					break;
				}
			}
			sb.Remove(searchStart, searchEnd - searchStart);

			return new ActionResult(searchStart, -1 * (searchEnd - searchStart), null);
		}
	}
}