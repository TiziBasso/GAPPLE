function focusInput(id) {
    document.getElementById(id).focus();
}

function BlurInput(id) {
    document.getElementById(id).blur();
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

function ExpandirMenu(id) {
    let nodoMostrar = document.getElementById(id)
    let nodoPadre = nodoMostrar
    let nivel = 0
    let padreEncontrado = false
    while (padreEncontrado == false) {
        if (nodoPadre.parentElement.id != 'padreMenu') {
            nivel++
            nodoPadre = nodoPadre.parentElement
        } else {
            nivel++
            padreEncontrado = true
        }
    }

    if (nodoMostrar.nextSibling != null) {
        if (nodoMostrar.getAttribute('expand') == 0) {
            nodoMostrar.setAttribute('expand', 1)
            document.getElementById(`icon${id}`).textContent = 'keyboard_arrow_up'
            let lis = Array.from(nodoMostrar.nextSibling.childNodes).filter(node => node.nodeType === Node.ELEMENT_NODE && node.tagName === 'LI')
            lis.forEach(function (args) {
                if (args.getAttribute('data-nodoVisible') == 1) {
                    args.setAttribute('data-nodoVisible', 0)
                } else {
                    args.setAttribute('data-nodoVisible', 1)
                    if (args.firstChild.tagName === 'A')
                        args.firstChild.style.padding = `10px 0 5px ${nivel * 30}px`
                    else
                        args.style.padding = `10px 0 5px ${nivel * 30}px`
                }
            });
        } else {
            nodoMostrar.setAttribute('expand', 0)
            document.getElementById(`icon${nodoMostrar.id}`).textContent = 'keyboard_arrow_down'
            let nodosAOcultar = Array.from(nodoMostrar.nextSibling.querySelectorAll('[data-nodovisible="1"]'))
            let nodosExpand = Array.from(nodoMostrar.nextSibling.querySelectorAll('[expand="1"]'))
            nodosAOcultar.forEach(function (args) {
                args.setAttribute('data-nodovisible', 0)
            });
            nodosExpand.forEach(function (args) {
                args.setAttribute('expand', 0)
                args.querySelector(`#icon${args.id}`).textContent = "keyboard_arrow_down"
            });
        }
    }
    OcultarItemsMenu(id)
}

function OcultarItemsMenu(id) {
    let nodoElegido = document.getElementById(id)

    if (nodoElegido.parentElement.id != 'padreMenu') {
        let lis = Array.from(nodoElegido.parentElement.childNodes).filter(node => node.nodeType === Node.ELEMENT_NODE && node.tagName === 'LI')
        let lis2 = Array.from(nodoElegido.nextElementSibling.childNodes).filter(node => node.nodeType === Node.ELEMENT_NODE && node.tagName === 'LI')
        let idsLis = lis.map(node => node.id);
        let idsLis2 = lis2.map(node => node.id);
        const idsAExcluir = new Set([...idsLis, ...idsLis2]);

        let nodosAOcultar = Array.from(document.querySelectorAll('[data-nodovisible="1"]')).filter(node => {
            return !idsAExcluir.has(node.id);
        });

        lis.forEach(function (args) {
            if (args.id != id && args.hasAttribute('expand')) {
                args.setAttribute('expand', 0)
                args.setAttribute('data-nodoactivo', 0)
                document.getElementById(`icon${args.id}`).textContent = "keyboard_arrow_down"
            }
        })

        nodosAOcultar.forEach(function (args) {
            args.setAttribute('data-nodovisible', 0)
            if (args.hasAttribute('expand')) {
                args.setAttribute('expand', 0)
                document.getElementById(`icon${args.id}`).textContent = "keyboard_arrow_down"
            }
        });
    } else {
        let lis2 = Array.from(nodoElegido.nextElementSibling.childNodes).filter(node => node.nodeType === Node.ELEMENT_NODE && node.tagName === 'LI')

        let idsLis2 = lis2.map(node => node.id);
        const idsAExcluir = new Set([...idsLis2]);

        let nodosAOcultar = Array.from(document.querySelectorAll('[data-nodovisible="1"]')).filter(node => {
            return !idsAExcluir.has(node.id);
        });

        nodosAOcultar.forEach(function (args) {
            args.setAttribute('data-nodovisible', 0)
            console.log(`icon${args}`)
            if (args.hasAttribute('expand')) {
                args.setAttribute('expand', 0)
                document.getElementById(`icon${args.id}`).textContent = "keyboard_arrow_down"
            }
        });
        if (nodosAOcultar.length > 0 && nodosAOcultar[0].parentElement.previousElementSibling.hasAttribute('expand')) {
            nodosAOcultar[0].parentElement.previousElementSibling.setAttribute('expand', 0)
            document.getElementById(`icon${nodosAOcultar[0].parentElement.previousElementSibling.id}`).textContent = "keyboard_arrow_down"
        }
    }
}

function ActivarNodoMenu(id) {
    let nodosActivos = document.querySelectorAll('li[data-nodoactivo="1"]')
    if (nodosActivos != null &&
        ((document.getElementById(id).getAttribute('expand') == 0 && document.getElementById(id).getAttribute('data-nodoactivo') == 1) ||
            (document.getElementById(id).getAttribute('expand') == null && document.getElementById(id).getAttribute('data-nodoactivo') == 0))) {
        nodosActivos.forEach(function (nodo) {
            nodo.setAttribute('data-nodoactivo', 0)
        });
    }
    let nodoActivar = document.getElementById(id)

    let nodosActivar = []
    let padreEncontrado = false
    while (padreEncontrado == false) {
        if (nodoActivar.parentElement.id != 'padreMenu') {
            if (nodoActivar.id == id) {
                nodosActivar.push(nodoActivar)
            } else {
                nodosActivar.push(nodoActivar.previousElementSibling)
            }
            nodoActivar = nodoActivar.parentElement
        } else {
            if (nodoActivar.id == id) {
                nodosActivar.push(nodoActivar)
            } else {
                nodosActivar.push(nodoActivar.previousElementSibling)
            }
            padreEncontrado = true
        }
    }
    for (let i = 0; i < nodosActivar.length; i++) {
        if (nodosActivar[i].firstChild.tagName === 'A') {
            nodosActivar[i].setAttribute('data-nodoactivo', 1)
        }
    }
}
function BuscarNodoActivo(uri) {
    let paginaActiva = document.querySelector(`a[href="${uri}"]`);
    if (paginaActiva != null) {
        paginaActiva.parentElement.setAttribute('data-nodoactivo', 1);
    }
}

document.addEventListener('keydown', function (e) {
    const target = e.target;
    const isEditable = target.tagName === "INPUT";

    if (!isEditable) return;

    const isArrowKey = ["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(e.key);
    const isPageDown = e.key === "PageDown";
    const isEnter = e.key === "Enter";

    const blockArrows = target.hasAttribute("arrowKeyBlocker");
    const blockPageDown = target.hasAttribute("pageDownBlocker");
    const blockEnter = target.hasAttribute("enterKeyBlocker");

    if ((blockArrows && isArrowKey) || (blockPageDown && isPageDown) || (blockEnter && isEnter)) {
        e.preventDefault();
    }
});

document.addEventListener('keydown', function (e) {
    const target = e.target;
    const isEditable = target.tagName === "INPUT";

    if (!isEditable) return;

    const isDecimalInput = target.getAttribute("data-validate") === "number";
    if (isDecimalInput) {
        if (e.ctrlKey) {
            const key = e.key.toLowerCase();
            if (key === 'v' || key === 'c' || key === 'z' || key === 'x') {
                return;
            }
        }

        if (e.key === "Dead" || e.key === "´") {
            e.preventDefault();
            return;
        }

        const allowedKeys = ["Enter", "Backspace", "Tab", "Delete", "ArrowLeft", "ArrowRight", "Home", "End", ".", ",", "-"];
        const isNumber = /^[0-9]$/.test(e.key);
        const isAllowedSymbol = allowedKeys.includes(e.key);

        if (!isNumber && !isAllowedSymbol) {
            e.preventDefault();
        }

        if ((e.key === '.' && target.value.includes('.')) ||
            (e.key === ',' && target.value.includes(',')) ||
            (e.key === '-' && target.value.includes('-'))) {
            e.preventDefault();
        }
    }
});

document.addEventListener('beforeinput', function (e) {
    const target = e.target;
    const isEditable = target.tagName === "INPUT";

    if (!isEditable) return;

    if (e.ctrlKey) {
        const key = e.key.toLowerCase();
        if (key === 'v' || key === 'c' || key === 'z' || key === 'x') {
            return;
        }
    }

    const isDecimalInput = target.getAttribute("data-validate") === "number";
    if (isDecimalInput) {
        // e.data puede ser null (como cuando presionás Backspace, etc.)
        const char = e.data;
        if (char && !/[0-9.,-]/.test(char)) {
            e.preventDefault();
        }
    }
});
