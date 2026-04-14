import { useEffect, useState } from 'react'
import { makeOrder } from '../Api'

export default function OrderPage() {
    const [message, setMessage] = useState('')
    const [error, setError] = useState('')
    const [orderData, setOrderData] = useState([])
    const [orderSubmitted, setOrderSubmitted] = useState(false)

    async function addPizzaToOrder() {
        const pizzaType = document.getElementById("pizzatype").value;
        const pizzaAmount = document.getElementById("pizzaamount").value;
        console.log(pizzaType, pizzaAmount);
        //var validatePizzaType = document.querySelector("#pizzatype option[value='" + pizzaType + "']");
        //var validatePizzaType = ("#pizzatype").find("option[value='" + pizzaType + "']");
        var validatePizzaType = document.getElementById("pizzatype").querySelector("option[value='" + pizzaType + "']");

        if (pizzaType && pizzaAmount && pizzaAmount > 0 && validatePizzaType) {
            setOrderData(order => order.push(`PizzaType: ${pizzaType} Pizza, Amount: ${pizzaAmount}`)); // change the format to something easy to display
        }
    }

    async function handleSubmit() {
        if (orderData == []) {
            // do nothing
        } else {
            // Convert the data in OrderData to fit the format the backend expects
            const formattedOrderData = orderData.map(item => {
                const [pizzaType, amount] = item.split(',').map(part => part.split(':')[1].trim());
                pizzaType.replace(/ /g, ''); // remove spaces from pizza type to match backend enum values
                return { PizzaType: pizzaType, Amount: parseInt(amount) };
            });

            try {
                const data = await makeOrder(formattedOrderData)
                setMessage(typeof data === 'string' ? data : JSON.stringify(data))
                setError('')
                setOrderSubmitted(true)

                // Print what has been ordered to the user, asking them to wait?
            } catch (err) {
                const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred'
                setError(errorMessage)
                setMessage('')
            }
        }   
    }

    return (<>
        {error ? (
            <div className="page">
                <div style={{ color: 'red' }}>
                    <p>Connection Error:</p>
                    <p>{error}</p>
                </div>
            </div>
        ) : orderSubmitted == false ? (
            <div className="orderpage">
                <h2>Order</h2>
                <div className="full-orderpage-display">
                    <form className="add-pizzas" onSubmit={(e) => e.preventDefault()}>
                        <div style={{ float: "left", width: "50%" }}>
                            <label htmlFor="pizzatype">Pizza Type</label>
                            <br />
                            <label htmlFor="pizzaamount">Amount</label>
                        </div>
                        <div style={{ float: "right", width: "50%" }}>
                            <input type="text" list="pizzaTypes" required={true} id="pizzatype" />
                            <datalist id="pizzaTypes">
                                <option value="Standard"></option>
                                <option value="Extremely Tasty"></option>
                                <option value="Odd"></option>
                                <option value="Rare"></option>
                                <option value="Horse Radish"></option>
                                <option value="Child"></option>
                            </datalist>
                            <br />
                            <input type="number" min="1" required={true} id="pizzaamount" />
                        </div>
                        <div style={{ width: "100%" }}>
                            <button type="submit" onSubmit={addPizzaToOrder}>Add to Order</button>
                        </div>
                    </form>
                    <div id="orderlist">
                        <h3>Current Order</h3>
                        <ul>
                            {orderData.map((item, index) => (
                                <li key={index}>{item}</li>
                            ))}
                        </ul>
                        <button type="submit" name="orderSubmitted" onClick={handleSubmit}>Place Order</button>
                    </div>
                </div>
            </div>
        ) : (
            <>
                <p>Loading...</p>
            </>
        )}
    </>)
}