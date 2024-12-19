function focusInput(id) {
    document.getElementById(id).focus();
}

function focusAndSelectInput(id) {
    document.getElementById(id).focus();
    document.getElementById(id).select();
}
async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

function downloadFileFromObject(filename, data) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = 'data:text/json;base64,' + data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function getItem(key) {
    let item = localStorage.getItem(key);
    return item;
}

function setItem(key, value) {
    localStorage.setItem(key, value);
}

function removeItem(key) {
    localStorage.removeItem(key);
}
function getCookie(name) {
    let nameEQ = name + "=";
    let ca = document.cookie.split(";");
    for (var i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === " ") c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length,
            c.length);
    }
    return null;
};


function onClickbyId(id) {
    document.getElementById(id).click();
}

function ScrollGrilla(id, scrollEnum) {
    var component = document.getElementById(id);
    var componentOverflow = component.querySelector("div");
    if (scrollEnum == 0)
        componentOverflow.scrollTop = 0;
    else
        componentOverflow.scrollTo(0, componentOverflow.scrollHeight);
}
function radzenNumberFocusAndSelectByName(name) {
    const input = document.getElementsByName(name)[0];
    input.focus();
    input.select();
}


function radzenNumberFocusAndSelectById(id) {
    const input = document.getElementById(id).children[0];
    input.focus();
    input.select();
}

function Imprimir(componentId) {
    var contenido = document.getElementById(componentId)
    if (contenido != null) {
        contenido = contenido.innerHTML;
        var iframe = document.createElement('iframe');
        document.body.appendChild(iframe);
        var doc = iframe.contentWindow.document;
        doc.write('<html><head><title>Impresión</title></head><body>' + contenido + '</body></html>');
        doc.close();
        iframe.contentWindow.print();
        iframe.remove();
    }
}
function radzenFocusByName(name) {
    const input = document.getElementsByName(name)[0];
    input.focus();
}
function radzenFocusAndSelectById(id) {
    const input = document.getElementById(id);
    input.focus();
    input.select();
}
function ScrolltwoGrids(id1, id2) {
    var component1 = document.getElementById(id1);
    var component2 = document.getElementById(id2);
    var componentOverflow1 = component1.querySelector("div");
    var componentOverflow2 = component2.querySelector("div");

    componentOverflow1.addEventListener('scroll', function () { componentOverflow2.scrollTop = this.scrollTop; })
    componentOverflow2.addEventListener('scroll', function () { componentOverflow1.scrollTop = this.scrollTop; })
}
function ScrollGridToIndex(idGrid, index) {
    setTimeout(function () {
        var grid = document.getElementById(idGrid);

        var offset = 70;

        if (grid.childElementCount > 1) {
            grid = grid.children[1];
        } else {
            grid = grid.children[0];
        }

        var element = grid.children[0].children[2].querySelector('.rz-data-row');
        var rectRow = element.getBoundingClientRect();
        var rowHeight = rectRow.bottom - rectRow.top;
        var elementPosition = rowHeight * (index + 1);
        var offsetPosition = elementPosition - offset;

        grid.scrollTo({
            top: offsetPosition,
            behavior: 'instant'
        });

    }, 100)
}
function AddStyleToClass(className, style, i) {
    var classElem = document.getElementsByClassName(className)
    classElem[i].style.cssText += style;
}
function HorizontalScrollGrilla(id, scrollEnum) {
    var component = document.getElementById(id);
    var componentOverflow = component.querySelector("div");
    if (scrollEnum == 2)
        componentOverflow.scrollLeft = 0;
    else
        componentOverflow.scrollTo(componentOverflow.scrollWidth, 0);
}
function ScrollGridToSelected(idGrid) {
    setTimeout(function () {
        var grid = document.getElementById(idGrid);
        var offset = 30;
        if (grid.childElementCount > 1) {
            grid = grid.children[1];
            offset = 70;
        } else {
            grid = grid.children[0];
        }
        var element = grid.querySelector("tbody").querySelector(".rz-state-highlight");
        if (element != null) {
            var bodyRect = grid.getBoundingClientRect().top;
            var elementRect = element.getBoundingClientRect().top;
            var elementPosition = elementRect - bodyRect;
            var offsetPosition = elementPosition - offset;

            grid.scrollTo({
                top: offsetPosition,
                behavior: 'instant'
            });
        }
    }, 100)
}
function navigationOnGrid(arrow, idGrid, cantCols) {
    const inputsArray = Array.from(document.getElementById(idGrid).querySelectorAll('input.editable-cell'));
    const currentIndex = inputsArray.indexOf(document.activeElement);

    let nextIndex;
    switch (arrow) {
        case 'ArrowRight':
            nextIndex = currentIndex + 1;
            break;
        case 'ArrowLeft':
            nextIndex = currentIndex - 1;
            break;
        case 'ArrowUp':
            nextIndex = currentIndex - cantCols;
            break;
        case 'ArrowDown':
        case 'Enter':
            nextIndex = currentIndex + cantCols;
            break;
    }

    while (nextIndex >= 0 && nextIndex < inputsArray.length) {
        const input = inputsArray[nextIndex];

        if (!input.disabled) {
            input.focus();
            input.select();
            return;
        }

        switch (arrow) {
            case 'ArrowRight':
                nextIndex++;
                break;
            case 'ArrowLeft':
                nextIndex--;
                break;
            case 'ArrowUp':
                nextIndex -= cantCols;
                break;
            case 'ArrowDown':
            case 'Enter':
                nextIndex += cantCols;
                break;
        }
    }
}