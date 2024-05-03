import { Button, ButtonGroup, Container, Nav, Navbar } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../Redux/Hooks";
import { logOut } from "../Redux/AuthSlice";

export function NavigationPage() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const isLogin = useAppSelector((state) => state.auth.isLogin);

    const handleLogout = () => {
        window.location.href = "/exit";
        dispatch(logOut());
    };

    return (
        <>
            <Navbar bg="light" data-bs-theme="light">
                <Container>
                    <Navbar.Brand>Paper street soap</Navbar.Brand>
                    <Nav className="m-auto">
                        <ButtonGroup>
                            <Button variant="primary" onClick={() => navigate("/home")}>Каталог</Button>
                            <Button variant="primary" onClick={() => navigate("/questions")}>Частые вопросы</Button>
                            <Button variant="primary" onClick={() => navigate("/about")}>О нас</Button>
                            <Button variant="primary" onClick={() => navigate("/contacts")}>Контакты</Button>
                            {isLogin ? (
                                <>
                                    <Button variant="primary" onClick={handleLogout}>Выйти</Button>
                                    <Button variant="primary" onClick={() => navigate("/cart")}>Корзина</Button>
                                </>
                            ) : (
                                <>
                                    <Button variant="primary" onClick={() => navigate("/login")}>Логин</Button>
                                    <Button variant="primary" onClick={() => navigate("/registration")}>Регистрация</Button>
                                </>
                            )}
                        </ButtonGroup>
                    </Nav>
                </Container>
            </Navbar>
        </>
    );
}