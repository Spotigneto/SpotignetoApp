// Simple UI helpers for Blazor interop
window.spotignete = window.spotignete || (function () {
  let resizeHandler = null;

  function getViewportWidth() {
    return window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth || 1024;
  }

  function registerResize(dotnetObj) {
    unregisterResize();
    resizeHandler = () => {
      try {
        dotnetObj.invokeMethodAsync('OnClientResized', getViewportWidth());
      } catch (e) {
        console.warn('Resize callback failed:', e);
      }
    };
    window.addEventListener('resize', resizeHandler);
  }

  function unregisterResize() {
    if (resizeHandler) {
      window.removeEventListener('resize', resizeHandler);
      resizeHandler = null;
    }
  }

  return {
    getViewportWidth,
    registerResize,
    unregisterResize,
  };
})();