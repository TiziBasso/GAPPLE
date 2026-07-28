function scrollGrilla(id, scrollEnum) {
    var component = document.getElementById(id);
    var componentOverflow = component.querySelector(".rz-data-grid-data");

    if (scrollEnum == 0)
        componentOverflow.scrollTop = 0;
    else
        componentOverflow.scrollTo(0, componentOverflow.scrollHeight);
}

function scrollDataList(id) {
    var component = document.getElementById(id);
    component.scrollTo(0, component.scrollHeight);
}

function scrollGridToIndex(idGrid, index) {
    setTimeout(function () {
        var grid = document.getElementById(idGrid);

        var offset = 35;

        if (grid.childElementCount > 1) {
            grid = grid.children[1];
            offset = 70;
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

function radzenNumberFocusAndSelectByName(name) {
    const input = document.getElementsByName(name)[0];
    if (input != null) {
        input.focus();
        input.select();
    }
}

function radzenNumberFocusAndSelectById(id) {
    const input = document.getElementById(id).children[0];
    if (input != null) {
        input.focus();
        input.select();
    }
}

window.radzenNumericPasteHandler = function (inputName) {
    const selector = `input[name='${inputName}']`;
    const input = document.querySelector(selector);

    if (!input) return;

    input.addEventListener("paste", function (e) {
        e.preventDefault();
        const text = (e.clipboardData || window.clipboardData).getData("text");

        let clean = text.replace(/[^\d.,]/g, "");

        const lastComma = clean.lastIndexOf(",");
        const lastDot = clean.lastIndexOf(".");
        let decimalSeparator = null;

        if (lastComma > lastDot) decimalSeparator = ",";
        else if (lastDot > lastComma) decimalSeparator = ".";

        if (decimalSeparator) {
            const regex = new RegExp(`[^\\d${decimalSeparator}]`, "g");
            clean = clean.replace(regex, "");
            clean = clean.replace(decimalSeparator, ".");
        } else {
            clean = clean.replace(/[^\d]/g, "");
        }

        const value = parseFloat(clean);
        if (!isNaN(value)) {
            this.value = value;
            this.dispatchEvent(new Event('input', { bubbles: true }));
            this.dispatchEvent(new Event('change', { bubbles: true }));
        }
    });
};


function radzenFocusAndSelectById(id) {
    const input = document.getElementById(id);
    input.focus();
    input.select();
}

function dataGridNavigation(arrow, idGrid, cantCols) {
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
    }, 200)
}