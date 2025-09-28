window.inputMaskHelper = {
    applyCurrencyMask: function (elementId) {
        var input = document.getElementById(elementId);
        if (input) {
            Inputmask({
                alias: "numeric",
                groupSeparator: ".",
                digits: 0,
                autoGroup: true,
                suffix: " vnđ",
                rightAlign: false,
                removeMaskOnSubmit: true
            }).mask(input);
        }
    }
};