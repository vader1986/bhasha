﻿using System;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class ChapterStatsDtoBuilder
    {
        public static ChapterStatsDto Build()
        {
            return new ChapterStatsDto {
                ChapterId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                Completed = Rnd.Create.Next(0, 1) == 0,
                Tips = Rnd.Create.NextString(8),
                Submits = Rnd.Create.NextString(8),
                Failures = Rnd.Create.NextString(8)
            };
        }
    }
}
