var alert = document.createElement("div");
alert.className = "header-alert";
alert.innerHTML = document.currentScript.dataset.message;

document.body.insertAdjacentElement("afterbegin", alert);