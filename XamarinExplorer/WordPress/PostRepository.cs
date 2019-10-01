﻿using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinExplorer.Services;
using System.Threading.Tasks;
using Shared.WordPress;

namespace XamarinExplorer
{
    public class PostRepository : Repository<WP_Post>
    {
        public override async Task<IEnumerable<WP_Post>> GetAsync(bool forceRefresh = false)
        {
            return await WordPressApi.Instance.GetPosts();
        }
    }
}