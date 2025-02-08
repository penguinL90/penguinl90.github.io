function setup_cursoreffect() {
    const media = window.matchMedia('(hover:hover)');
    if (!media.matches) return;
    const element = document.getElementsByClassName("cursor-effect");
    for (let i = 0; i < element.length; i++) {
        const el = element[i];
        const overlay = document.createElement("img");
        if (el.dataset["ceSetuped"] === '1') {
            continue;
        }
        overlay.style.transition = "opacity 0.3s";
        overlay.style.opacity = 0;
        overlay.src = "/img/cursoroverlay.svg";
        overlay.style.position = "absolute";
        overlay.style.pointerEvents = "none";
        el.appendChild(overlay);

        const wholehandler = () => {
            const width = el.offsetWidth;
            const height = el.offsetHeight;
            const overlayhalfsize = Math.max(width, height);

            const mouseleave = () => {
                el.removeEventListener("mousemove", mousemove);
                el.removeEventListener("mouseleave", mouseleave);
                overlay.style.opacity = 0;
            };

            const mousemove = (event) => {
                const bd = el.getBoundingClientRect();
                overlay.style.left = event.clientX - bd.left - overlayhalfsize + "px";
                overlay.style.top = event.clientY - bd.top - overlayhalfsize + "px";
            };

            overlay.style.opacity = 0.5;
 
            overlay.height = overlayhalfsize * 2;
            overlay.width = overlayhalfsize * 2;

            el.addEventListener("mouseleave", mouseleave);
            el.addEventListener("mousemove", mousemove);
        }

        el.addEventListener("mouseenter", wholehandler);
        el.dataset["ceSetuped"] = '1';
    }
}

function copytoclipboard(txt) {
    navigator.clipboard.writeText(txt);
}

function log(txt) {
    console.log(txt);
}

function scroll(elementID) {
    document.getElementById(elementID).scrollIntoView({ behavior: "smooth" });
}

function logscrollupdate() {
    const logl = document.getElementById("logl");
    setTimeout(() => {
        logl.scroll({
            top: logl.scrollHeight,
            behavior: "smooth"
        });
    }, 50);
}

function scrollrect(element) {
    document.getElementById(element).scrollIntoView({ behavior: "smooth", inline: "nearest", block: "nearest" });
}

function popuprect(element) {
    const ele = document.getElementById(element);
    ele.classList.add("popupani");
    setTimeout(() => {
        ele.classList.remove("popupani");
    }, 200);
}