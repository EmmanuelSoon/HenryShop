window.onload = function () {
    let adding = document.getElementById("AddQuantity");
    let minus = document.getElementById("MinusQuantity");
    let remove = document.getElementById("RemoveItem");


    adding.onclick = function () {
        PlusToCart(adding.value)
    }

    minus.onclick = function () {
        MinusFromCart(minus.value);
    }

    remove.onclick = function () {
        RemoveFromCart1(remove.value);
    }
}






function PlusToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/PlusToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                // convert server's JSON string to a JavaScript object
                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}

function MinusFromCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/MinusFromCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}


function RemoveFromCart1(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/RemoveFromCart1");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}