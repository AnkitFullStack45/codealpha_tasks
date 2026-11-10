const currentDisplay = document.getElementById("current");
const previousDisplay = document.getElementById("previous");
let currentValue = "0";
let previousValue = "";
let operation = null;
let resetScreen = false;
function updateDisplay() {
    currentDisplay.textContent = currentValue;
    previousDisplay.textContent =
        previousValue && operation
            ? `${previousValue} ${operation}`
            : "";
}
/* Add number */
function appendNumber(number) {
    if (resetScreen) {
        currentValue = "";
        resetScreen = false;
    }
    // Prevent multiple decimal points
    if (number === "." && currentValue.includes(".")) {
        return;
    }
    // Prevent leading zeros
    if (currentValue === "0" && number !== ".") {
        currentValue = number;
    } else {
        currentValue += number;
    }
    updateDisplay();
}
/* Select operation */
function chooseOperation(selectedOperation) {
    if (currentValue === "") {
        return;
    }
    if (previousValue !== "") {
        calculate();
    }
    operation = selectedOperation;
    previousValue = currentValue;
    resetScreen = true;
    updateDisplay();
}
/* Calculate */
function calculate() {
    if (previousValue === "" || operation === null) {
        return;
    }
    const previous = parseFloat(previousValue);
    const current = parseFloat(currentValue);
    let result;
    switch (operation) {
        case "+":
            result = previous + current;
            break;
        case "−":
            result = previous - current;
            break;

        case "×":
            result = previous * current;
            break;
        case "÷":
            if (current === 0) {
                currentValue = "Error";
                previousValue = "";
                operation = null;
                updateDisplay();
                return;
            }
            result = previous / current;
            break;
    }
    // Remove floating-point precision problems
    result = Math.round((result + Number.EPSILON) * 100000000) / 100000000;
    currentValue = result.toString();
    previousValue = "";
    operation = null;
    resetScreen = true;
    updateDisplay();
}
/* Clear calculator */
function clearCalculator() {
    currentValue = "0";
    previousValue = "";
    operation = null;
    resetScreen = false;
    updateDisplay();
}
/* Delete last character */
function deleteNumber() {
    if (currentValue === "Error") {
        clearCalculator();
        return;
    }
    if (currentValue.length === 1) {
        currentValue = "0";
    } else {
        currentValue = currentValue.slice(0, -1);
    }
    updateDisplay();
}
/* Percentage */
function percentage() {
    let number = parseFloat(currentValue);
    number = number / 100;
    currentValue = number.toString();
    updateDisplay();
}
/* Button events */
document.querySelectorAll("[data-number]").forEach(button => {
    button.addEventListener("click", () => {
        appendNumber(button.dataset.number);
    });
});
document.querySelectorAll("[data-operation]").forEach(button => {
    button.addEventListener("click", () => {
    chooseOperation(button.dataset.operation);
    });
});
document.querySelector('[data-action="equals"]')
    .addEventListener("click", calculate);
document.querySelector('[data-action="clear"]')
    .addEventListener("click", clearCalculator);
document.querySelector('[data-action="delete"]')
    .addEventListener("click", deleteNumber);
document.querySelector('[data-action="percent"]')
    .addEventListener("click", percentage);
/* Keyboard support */
document.addEventListener("keydown", event => {
    const key = event.key;
    if (!isNaN(key) || key === ".") {
        appendNumber(key);
    }
    else if (
        key === "+" ||
        key === "-" ||
        key === "*" ||
        key === "/"
    ) {
        let operator = key;
        if (key === "*") operator = "×";
        if (key === "/") operator = "÷";
        if (key === "-") operator = "−";
        chooseOperation(operator);
    }
    else if (key === "Enter" || key === "=") {
        calculate();
    }
    else if (key === "Backspace") {
        deleteNumber();
    }
    else if (key === "Escape") {
        clearCalculator();
    }
    else if (key === "%") {
        percentage();
    }
});   