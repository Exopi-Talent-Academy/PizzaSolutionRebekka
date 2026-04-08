import { useEffect, useState } from 'react'
import { makeOrder } from '../Api'

export default function OrderPage() {
    const [message, setMessage] = useState('')
    const [error, setError] = useState('')
    const [orderData, setOrderData] = useState([])
    const [submitOrder, setSubmitOrder] = useState(false)

    async function addPizzaToOrder() {
        const pizzaType = document.getElementById("pizzatype").value;
        const pizzaAmount = document.getElementById("pizzaamount").value;

        if (pizzaType && pizzaAmount && pizzaAmount > 0) {
            setOrderData(order => order.concat([`PizzaType: ${pizzaType} Pizza, Amount: ${pizzaAmount}`])); // change the format to something easy to display
        }
    }

    async function handleSubmit() {
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
            setSubmitOrder(true)

            // Print what has been ordered to the user, asking them to wait?
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred'
            setError(errorMessage)
            setMessage('')
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
        ) : submitOrder == false ? (
            <div className="page">
                <h2>Order</h2>
                <p>Here you can place your order.</p>
                <div>
                    <label htmlFor="pizzatype">Pizza Type:</label>
                    <input list="pizzaTypes" required={true} id="pizzatype" />
                    <datalist id="pizzaTypes">
                        <option value="Standard"></option>
                        <option value="Extremely Tasty"></option>
                        <option value="Odd"></option>
                        <option value="Rare"></option>
                        <option value="Horse Radish"></option>
                        <option value="Child"></option>
                    </datalist>
                    <br />
                    <label htmlFor="pizzaamount">Amount:</label>
                    <input type="number" min="1" required={true} id="pizzaamount" />
                    <br />
                    <button type="submit" onClick={addPizzaToOrder}>Add to Order</button>
                </div>
                <div>
                    <h3>Current Order:</h3>
                    <ul>
                        {orderData.map((item, index) => (
                            <li key={index}>{item}</li>
                        ))}
                    </ul>
                </div>
                <button type="submit" name="submitOrder" onClick={handleSubmit}>Place Order</button>
            </div>
        ) : (
            <>
                <p>Loading...</p>
            </>
        )}
    </>)
}