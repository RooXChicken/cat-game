document.body.style.backgroundColor = "black";

const image = document.createElement("img");
image.src = "coffee.png";
image.style.position = "fixed";
image.id = "coffee";
image.style.imageRendering = "pixelated";
document.body.appendChild(image);

var x = 0;
var y = 0;

var leftPressed = false;
var rightPressed = false;
var downPressed = false;
var upPressed = false;

document.addEventListener('keydown', function(event) {
    if(event.key == "a") {
        leftPressed = true;
    }
    else if(event.key == "d") {
        rightPressed = true;
    }
    if(event.key == "w") {
        upPressed = true;
    }
    else if(event.key == "s") {
        downPressed = true;
    }
});

document.addEventListener('keyup', function(event) {
    if(event.key == "a") {
        leftPressed = false;
    }
    else if(event.key == "d") {
        rightPressed = false;
    }
    if(event.key == "w") {
        upPressed = false;
    }
    else if(event.key == "s") {
        downPressed = false;
    }
});

setInterval(run, 12);

function run()
{
    if(leftPressed)
        x--;
    if(rightPressed)
        x++;

    if(upPressed)
        y--;
    if(downPressed)
        y++;

    document.getElementById("coffee").style.left = x + "px";
    document.getElementById("coffee").style.top = y + "px";
}