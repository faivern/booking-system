
export default function Navbar() {
    return (    
<div>
    <nav className="bg-blue-500 p-4">
        <ul className="flex space-x-4 text-white">
            <li><a href="/" className="hover:underline">Home</a></li>
            <li><a href="/about" className="hover:underline">About</a></li>
            <li><a href="/contact" className="hover:underline">Contact</a></li>
        </ul>
    </nav>
</div>
    );
}