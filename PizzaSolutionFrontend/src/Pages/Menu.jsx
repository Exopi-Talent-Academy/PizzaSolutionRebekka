import { useEffect, useState } from 'react'
import { getMenu } from '../Api'

export default function MenuPage() {
    const [message, setMessage] = useState('')
    const [error, setError] = useState('')

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
                <div className='article-padding'>
                    <p className='page-title'>{message}</p>
                </div>
            </div>
        ) : (
            <>
                <p>Loading...</p>
            </>
        )}
    </>)
}