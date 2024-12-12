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

window.onload = function iconoTema() {
    var link1 = document.createElement('link');
    var head = document.head;

    var currentDate = new Date();
    var timestamp = currentDate.getTime();

    if (getCookie('tema') == null || getCookie('tema') == 'light') {
        if (getCookie('tema') == null) {
            document.cookie = "tema=light;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
        }

        link1.rel = 'stylesheet';
        //link1.href = './css/component-light.css';
        link1.href = 'RadzenStyles/css/Material3.css?v=' + timestamp;
        head.insertBefore(link1, head.firstChild);
    } else {
        document.body.className = 'dark-theme';

        link1.rel = 'stylesheet';
        //link1.href = './_content/Radzen.Blazor/css/dark.css';
        link1.href = 'RadzenStyles/css/Material3-Dark.css?v=' + timestamp;
        head.insertBefore(link1, head.firstChild);
    }
}

function changetheme(modoOscuro) {
    if (modoOscuro) {
        document.cookie = "tema=dark;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
    } else {
        document.cookie = "tema=light;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
    }
    location.reload();
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

window.onload = function iconoTema() {
    var link1 = document.createElement('link');
    var head = document.head;

    var currentDate = new Date();
    var timestamp = currentDate.getTime();

    if (getCookie('tema') == null || getCookie('tema') == 'light') {
        if (getCookie('tema') == null) {
            document.cookie = "tema=light;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
        }
       
        link1.rel = 'stylesheet';
        //link1.href = './css/component-light.css';
        link1.href = 'RadzenStyles/css/Material3.css?v=' + timestamp;
        head.insertBefore(link1, head.firstChild);
    } else {
        document.body.className = 'dark-theme';
        
        link1.rel = 'stylesheet';
        //link1.href = './_content/Radzen.Blazor/css/dark.css';
        link1.href = 'RadzenStyles/css/Material3-Dark.css?v=' + timestamp;
        head.insertBefore(link1, head.firstChild);
    }
}

function changetheme(modoOscuro) {
    if (modoOscuro) {
        document.cookie = "tema=dark;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
    } else {
        document.cookie = "tema=light;path=/;SameSite=lax;expires=Thu, 12 Aug 2049 20:47:11 UTC;";
    }
    location.reload();
}

document.addEventListener('keydown', function (event) {
    // Selecciona el elemento que contiene el grid
    let gridElement = document.querySelector('.OrdenProductos');

    // Obtiene el valor de la propiedad grid-template-columns
    const gridStyles = window.getComputedStyle(gridElement);
    const columns = gridStyles.getPropertyValue('grid-template-columns');

    // Cuenta cuántas columnas hay (dividiendo por el espacio que hay entre las columnas)
    const numberOfColumns = columns.split(' ').length;

    if (event.target.tagName.toLowerCase() === 'input') {
        let currentInput = event.target;
        let currentId = currentInput.id;
        let parts = currentId.split('_');
        let row = parseInt(parts[1]);
        let col = parseInt(parts[2]);

        if (event.key === 'ArrowRight' || event.key === 'ArrowLeft' || event.key === 'ArrowDown' || event.key === 'ArrowUp') {
            if (event.key === 'ArrowRight') {
                if (document.getElementById(`input_${row + 1}_${col}`) != null) {
                    row++;
                } else {
                    let colAux = 0;
                    while (true) {
                        if (document.getElementById(`input_${row + 1}_${colAux}`) != null) {
                            colAux++;
                        }
                        else {
                            col = colAux - 1;
                            row++;
                            break;
                        }
                    }
                }
            } else if (event.key === 'ArrowLeft') {
                if (document.getElementById(`input_${row - 1}_${col}`) != null) {
                    row--;
                } else {
                    let colAux = 0;
                    while (true) {
                        if (document.getElementById(`input_${row - 1}_${colAux}`) != null) {
                            colAux++;
                        }
                        else {
                            col = colAux - 1;
                            row--;
                            break;
                        }
                    }
                }
            } else if (event.key === 'ArrowDown') {
                if (document.getElementById(`input_${row}_${col + 1}`) != null) {
                    col++;
                }
                else {
                    row += numberOfColumns;
                    col = 0;
                }
            } else if (event.key === 'ArrowUp') {
                if (document.getElementById(`input_${row}_${col - 1}`) != null) {
                    col--;
                } else {
                    row -= numberOfColumns;
                    let colAux = 0;
                    while (true) {
                        if (document.getElementById(`input_${row}_${colAux}`) != null) {
                            colAux++;
                        }
                        else {
                            col = colAux - 1;
                            break;
                        }
                    }
                }
            }
        if (event.key === 'ArrowRight') {
            if (document.getElementById(`input_${row + 1}_${col}`) != null) {
                row++;
            } else {
                let colAux = 0;
                while (true) {
                    if (document.getElementById(`input_${row + 1}_${colAux}`) != null) {
                        colAux++;
                    }
                    else {
                        col = colAux - 1;
                        row++;
                        break;
                    }
                }
            }
        } else if (event.key === 'ArrowLeft') {
            if (document.getElementById(`input_${row - 1}_${col}`) != null) {
                row--;
            } else {
                let colAux = 0;
                while (true) {
                    if (document.getElementById(`input_${row - 1}_${colAux}`) != null) {
                        colAux++;
                    }
                    else {
                        col = colAux - 1;
                        row--;
                        break;
                    }
                }
            }
        } else if (event.key === 'ArrowDown') {
            if (document.getElementById(`input_${row}_${col + 1}`) != null) {
                col++;
            }
            else {
                row += 6;
                col = 0;
            }
        } else if (event.key === 'ArrowUp') {
            if (document.getElementById(`input_${row}_${col - 1}`) != null) {
                col--;
            } else {
                row -= 6;
                let colAux = 0;
                while (true) {
                    if (document.getElementById(`input_${row}_${colAux}`) != null) {
                        colAux++;
                    }
                    else {
                        col = colAux - 1;
                        break;
                    }
                }
            }
        }

            // Asegúrate de que la siguiente posición sea válida
            let nextInputId = `input_${row}_${col}`;
            let nextInput = document.getElementById(nextInputId);

            if (nextInput) {
                nextInput.focus();
            }
            event.preventDefault(); // Previene la acción por defecto de la tecla
        }
    }
});