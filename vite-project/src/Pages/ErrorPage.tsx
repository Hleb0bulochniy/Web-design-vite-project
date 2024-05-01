import { useParams } from "react-router-dom"

export function ErrorPage() {
    const { id } = useParams();
    return <h1>Ошибка {id}</h1>
}