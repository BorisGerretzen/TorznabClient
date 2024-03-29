﻿using TorznabClient.Models;

namespace TorznabClient.Torznab;

public interface ITorznabClient
{
    /// <summary>
    ///     List the supported features, protocol version, and other metadata about the indexer.
    /// </summary>
    /// <param name="apiKey">API key to use.</param>
    /// <param name="url">Overrides configured url.</param>
    Task<TorznabCaps> GetCapsAsync(
        string? apiKey = null,
        string? url = null
    );

    /// <summary>
    ///     Searches the index for items matching the search criteria.
    /// </summary>
    /// <param name="apiKey">API key to use.</param>
    /// <param name="query">Search input, case insensitive.</param>
    /// <param name="groups">List of usenet groups.</param>
    /// <param name="limit">Upper limit of items to be returned.</param>
    /// <param name="categories">List of categories.</param>
    /// <param name="attributes">List of extended attributes.</param>
    /// <param name="extended">List all extended attributes (<see cref="attributes" /> is ignored)</param>
    /// <param name="delete">Delete the item from the user's cart on download.</param>
    /// <param name="maxAge">Only returns results posted in the last n days.</param>
    /// <param name="minSize">Only return results which have a size greater than this (bytes).</param>
    /// <param name="maxSize">Only return results which have a size smaller than this (bytes).</param>
    /// <param name="offset">The 0 based query offset defining which part of the response we want.</param>
    /// <param name="sort">
    ///     The sorting of the data. Available options being cat, name, size, files, states, in the format
    ///     'value_asc', 'cat_desc', etc.
    /// </param>
    /// <param name="url">Overrides configured url.</param>
    Task<TorznabRss> SearchAsync(
        string? apiKey = null,
        string? query = null,
        IEnumerable<string>? groups = null,
        int? limit = null,
        IEnumerable<int>? categories = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        long? minSize = null,
        long? maxSize = null,
        int? offset = null,
        string? sort = null,
        string? url = null
    );

    /// <summary>
    ///     Searches the index in the TV category for items matching the search criteria.
    ///     The criteria includes query string and in addition information about season and episode.
    /// </summary>
    /// <param name="apiKey">API key to use.</param>
    /// <param name="query">Search input, case insensitive.</param>
    /// <param name="season">Season string, e.g. S13 or 13.</param>
    /// <param name="episode">Episode string, e.g. E13 or 13.</param>
    /// <param name="limit">Upper limit of items to be returned.</param>
    /// <param name="tvRageId">TVRage id of the item being queried.</param>
    /// <param name="tvMazeId">TVMaze id of the item being queried.</param>
    /// <param name="tvDbId">TVDB id of the item being queried.</param>
    /// <param name="categories">List of categories.</param>
    /// <param name="attributes">List of extended attributes.</param>
    /// <param name="extended">List all extended attributes (<see cref="attributes" /> is ignored)</param>
    /// <param name="delete">Delete the item from the user's cart on download.</param>
    /// <param name="maxAge">Only returns results posted in the last n days.</param>
    /// <param name="offset">The 0 based query offset defining which part of the response we want.</param>
    /// <param name="url">Overrides configured url.</param>
    Task<TorznabRss> TvSearchAsync(
        string? apiKey = null,
        string? query = null,
        string? season = null,
        string? episode = null,
        int? limit = null,
        string? tvRageId = null,
        int? tvMazeId = null,
        int? tvDbId = null,
        IEnumerable<int>? categories = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        int? offset = null,
        string? url = null
    );

    /// <summary>
    ///     Searches the index for items matching an IMDb ID or search query.
    /// </summary>
    /// <param name="apiKey">API key to use.</param>
    /// <param name="query">Search input, case insensitive.</param>
    /// <param name="imdbId">IMDb ID of the item being queried.</param>
    /// <param name="categories">List of categories.</param>
    /// <param name="genre">A genre string, e.g. 'Romance' would match '(Comedy, Drama, Indie, Romance)'</param>
    /// <param name="attributes">List of extended attributes.</param>
    /// <param name="extended">List all extended attributes (<see cref="attributes" /> is ignored)</param>
    /// <param name="delete">Delete the item from the user's cart on download.</param>
    /// <param name="maxAge">Only returns results posted in the last n days.</param>
    /// <param name="offset">The 0 based query offset defining which part of the response we want.</param>
    /// <param name="url">Overrides configured url.</param>
    Task<TorznabRss> MovieSearchAsync(
        string? apiKey = null,
        string? query = null,
        string? imdbId = null,
        IEnumerable<int>? categories = null,
        string? genre = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        int? offset = null,
        string? url = null
    );
}