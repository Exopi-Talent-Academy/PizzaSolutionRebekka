import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import WelcomePage from './Pages/Welcome'
import MenuPage from './Pages/Menu'
import OrderPage from './Pages/Order'

function App() {
  const restaurantName = "Mario's Pizzaria";
  const [currentPage, setCurrentPage] = useState("welcome");

  return (
    <>
    <title>{restaurantName}</title>
    <header className='navigation'>
      <h1>{restaurantName}</h1>
      <button onClick={() => setCurrentPage("welcome")}>Welcome</button>
      <button onClick={() => setCurrentPage("menu")}>Menu</button>
      <button onClick={() => setCurrentPage("order")}>Order</button>
      <button onClick={() => setCurrentPage("recipe")}>Recipes</button>
      <button onClick={() => setCurrentPage("restocking")}>Restock</button>
    </header>
    <div>
      {getPage(currentPage)}
    </div>
    </>
  )
}

export default App

function getPage(page) {
    switch (page) {
      case "welcome":
        return <WelcomePage></WelcomePage>
      case "menu":
        return <MenuPage></MenuPage>
      case "order":
        return <OrderPage></OrderPage>
      case "recipe":
        return <p>Recipe page coming soon...</p>
      case "restocking":
        return <p>Restocking page coming soon...</p>
      default:
        return <WelcomePage></WelcomePage>
    }
}