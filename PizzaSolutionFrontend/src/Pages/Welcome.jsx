import { useEffect, useState } from 'react'
import { getWelcome } from '../Api'

export default function WelcomePage() {
    const [message, setMessage] = useState('')
    const [error, setError] = useState('')

    useEffect(() => {
        const fetchWelcome = async () => {
            try {
                const data = await getWelcome()
                setMessage(typeof data === 'string' ? data : JSON.stringify(data))
                setError('')
            } catch (err) {
                const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred'
                setError(errorMessage)
                setMessage('')
            }
        }

        fetchWelcome()
    }, [])

    return (<>
        {error ? (
            <div className="page">
                <div style={{ color: 'red' }}>
                    <p>Connection Error:</p>
                    <p>{error}</p>
                </div>
            </div>
        ) : message ? (
            <div className="page">
                <div className="welcome-message">
                    <p>{message}</p>
                </div>
            </div>
        ) : (
            <>
                <p>Loading...</p>
            </>
        )}
    </>)
}