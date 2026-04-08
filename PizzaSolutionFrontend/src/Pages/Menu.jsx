import { use, useEffect, useState } from 'react'
import { getMenu } from '../Api'

export default function MenuPage() {
    const [message, setMessage] = useState('')
    const [error, setError] = useState('')
    const [menuTitle, setMenuTitle] = useState('')
    const [menuData, setMenuData] = useState([])

    useEffect(() => {
        const fetchMenu = async () => {
            try {
                const data = await getMenu()
                setMessage(typeof data === 'string' ? data : JSON.stringify(data))
                setError('')
            } catch (err) {
                const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred'
                setError(errorMessage)
                setMessage('')
            }
        }

        fetchMenu()
    }, [])

    useEffect(() => {
        if (message) {
            const obj = JSON.parse(message);
            setMenuTitle(obj.title);

            //console.log(message);

            var sortedPizzas = obj.items.sort((a, b) => a.pizzaType.localeCompare(b.pizzaType)); // Sort pizzas alphabetically by pizza type

            var pizzas = [];
            sortedPizzas.forEach(pizza => {
                var description = pizza.description;
                var pizzaType = pizza.pizzaType;
                var price = pizza.price;
                var soldOut = pizza.soldOut;
                pizzas.push(`Description: ${description}, PizzaType: ${pizzaType}, Price: ${price}, SoldOut: ${soldOut}`);
            });

            setMenuData(pizzas);
        }
    }, [message])

    return (<>
        {error ? (
            <div className="page">
                <div style={{ color: 'red' }}>
                    <p>Connection Error:</p>
                    <p>{error}</p>
                </div>
            </div>
        ) : message ? (
            <div className="menupage">
                <h2 id='menu-title'>{menuTitle}</h2>
                <div id="menu-display">{menuData.map((pizza) => (<>
                                                                    <div className='menu-card'>{displayPizza(pizza)}</div> 
                                                                 </>))}
                </div>
            </div>
        ) : (
            <>
                <p>Loading...</p>
            </>
        )}
    </>)
}

function displayPizza(pizza) {
    var splitstring = [];
    var pizzaType = "";

    if (typeof pizza === 'string') {
        splitstring = pizza.split(',').map(part => part.split(':')[1].trim());
        pizzaType = splitstring[1].replace(/([a-z])([A-Z])/g, '$1 $2'); // Add space before capital letters for better display
    } else {
        // Needs to not throw an error and stop because it needs a few tries
        console.log("Pizza data is not a string");
    }

    return (<>
        <table className='pizza-description'>
            <tbody>
                <tr>
                    <th colSpan="2">{splitstring[0]}</th>
                </tr>
                <tr>
                    <td colSpan="2" id="pizza-type">{pizzaType}</td>
                </tr>
                <tr>
                    <td>${splitstring[2]}</td>
                    <td className="order-or-sold-out">{checksoldOut(splitstring[3])}</td>
                </tr>
            </tbody>
        </table>
    </>);
}

function checksoldOut(soldOut) {
    if (soldOut === "true") {
        return (<>
                    <p id="sold-out">Sold Out</p> 
                </>);
    } else {
        return (<>
                    <button>Order</button>
                </>); //when the button is clicked, it should go to the order page with the correct pizza type already selected
    }
}