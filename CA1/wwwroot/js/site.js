

function PlusToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "CheckOut/PlusToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.reponseText);

                if (data.status == "success") {
                    window.location.href = "CheckOut/Index";
                }
            }
        }
    }

    let req = { ProductId: id }

    xhr.send(JSON.stringify(req));
}

function MinusFromCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "CheckOut/MinusFromCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.reponseText);

                if (data.status == "success") {
                    window.location.href = "CheckOut/Index";
                }
            }
        }
    }

    let req = { ProductId: id }

    xhr.send(JSON.stringify(req));
}
function RemoveFromCart(id)
{
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "CheckOut/RemoveFromCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function ()
    {
        if (this.readyState == XMLHttpRequest.DONE)
        {
            if (this.status == 200)
            {
                let data = JSON.parse(this.reponseText);

                if (data.status == "success")
                {
                    window.location.href = "CheckOut/Index";
                }
            }
        }
    }

    let req = { ProductId: id }

    xhr.send(JSON.stringify(req));
}