export function mostrarVideo(videoId){
    var videoContainer = document.getElementById('videoContainer');

    videoContainer.innerHTML = '';

    var videoElement = document.createElement('iframe');
    switch (videoId) {
        case 1:
            videoElement.src = 'https://www.youtube.com/embed/NHhTMh0nURA';
            break;
        case 2:
            videoElement.src = 'https://www.youtube.com/embed/dbf-xGWJ4wA';
            break;
        case 3:
            videoElement.src = 'https://www.youtube.com/embed/rqDZ_W_r52w';
            break;
        case 4:
            videoElement.src = 'https://www.youtube.com/embed/ahjt7MuJEvg';
            break;
        default:
            break;
    }
    videoElement.style.width = "760px";
    videoElement.style.height = "515px";
    videoContainer.appendChild(videoElement);
};