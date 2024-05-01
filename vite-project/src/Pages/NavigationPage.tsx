import { Button, Container, Nav, Navbar } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

export function NavigationPage() {
    const s = useNavigate();
    return(
        <>
        <Navbar bg="light" data-bs-theme="light">
        <Container>
            <Navbar.Brand href="#home">Navbar</Navbar.Brand>
            <Nav className="me-auto">
                <Nav.Link href="/home">Home</Nav.Link>
                <Nav.Link href="/features">Features</Nav.Link>
                <Nav.Link href="#pricing">Pricing</Nav.Link>
            </Nav>
        </Container>
    </Navbar>
    <Button variant="primary" onClick={() => s("/error")}></Button>
        </>
    )
}