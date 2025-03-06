let images = document.querySelectorAll(".imgSmall img");
let bigImg = document.getElementById("bigImg")

if (images.length > 0) {
    for (let i = 0; i < images.length; i++) {
        images[i].addEventListener("mouseover", () => {
            bigImg.setAttribute("src", images[i].src);
        })
    }
}