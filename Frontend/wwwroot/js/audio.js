let audio = new Audio();
audio.preload = 'none';
audio.crossOrigin = 'anonymous';

export function setSource(url) {
  audio.src = url;
}

export function play() {
  return audio.play();
}

export function pause() {
  audio.pause();
}

export function stop() {
  audio.pause();
  audio.currentTime = 0;
}

export function attach(dotnetRef) {
  audio.ontimeupdate = () => {
    const cur = audio.currentTime || 0;
    const tot = isFinite(audio.duration) ? audio.duration : 0;
    dotnetRef.invokeMethodAsync('OnAudioTimeUpdate', cur, tot);
  };
  audio.onended = () => {
    dotnetRef.invokeMethodAsync('OnAudioEnded');
  };
}

export function seek(seconds) {
  audio.currentTime = seconds;
}

export function setVolume(v) {
  audio.volume = v;
}