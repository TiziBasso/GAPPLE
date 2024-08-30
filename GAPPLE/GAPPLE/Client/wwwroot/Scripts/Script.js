document.addEventListener('keydown', function (event) {
    if (event.target.tagName.toLowerCase() === 'input') {
        let currentInput = event.target;
        let currentId = currentInput.id;
        let parts = currentId.split('_');
        let row = parseInt(parts[1]);
        let col = parseInt(parts[2]);

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
                row += 4;
                col = 0;
            }
        } else if (event.key === 'ArrowUp') {
            if (document.getElementById(`input_${row}_${col - 1}`) != null) {
                col--;
            } else {
                row -= 4;
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
            event.preventDefault(); // Previene la acción por defecto de la tecla
        }
    }
});