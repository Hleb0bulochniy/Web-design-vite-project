import { Button } from "react-bootstrap";

export function ContactsPage() {
    return (
        <>
            <h1></h1>
            <h6>Телефон: +71234567890</h6>
            <h6>Почта: IvanTriple@gmail.com</h6>
            <h6>Адреса:Москва, ул. Большая Семёновская, 38</h6>
            <h6>Москва, ул. Прянишникова, 2а</h6>
            <Button variant="link" onClick={() => window.location.href = "https://vk.com/"}>Вконтакте</Button>
            <Button variant="link" onClick={() => window.location.href = "https://telegram.org/"}>Телеграм</Button>
        </>
    )
}