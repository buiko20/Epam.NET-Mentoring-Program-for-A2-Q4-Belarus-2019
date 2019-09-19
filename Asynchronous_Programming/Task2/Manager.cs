using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    internal class Manager : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IDictionary<SiteData, CancellationTokenSource> _sources = new Dictionary<SiteData, CancellationTokenSource>();
        private bool _isDisposed;

        public async Task<SiteData> LoadAsync(string uri)
        {
            ThrowIfDisposed();
            try
            {
                var cts = new CancellationTokenSource();
                var result = new SiteData(Guid.NewGuid(), uri);
                _sources.Add(result, cts);

                result.Html = await LoadAsync(uri, cts.Token).ConfigureAwait(false);
                return result;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public void Cancel(Guid guid)
        {
            ThrowIfDisposed();
            var source = _sources.FirstOrDefault(pair => pair.Key.Guid == guid);
            source.Value?.Cancel();
            source.Value?.Dispose();
            _sources.Remove(source);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var source in _sources)
                {
                    source.Value.Cancel();
                    source.Value.Dispose();
                }

                _httpClient?.Dispose();
            }

            _isDisposed = true;
        }

        private async Task<string> LoadAsync(string uri, CancellationToken token)
        {
            await Task.Delay(5000, token).ConfigureAwait(false);
            var result = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false);
            string html = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return html;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(Manager));
        }
    }
}
