document.addEventListener("DOMContentLoaded", function () {
    const highlightText = new URLSearchParams(window.location.search).get('highlight');

    if (highlightText) {
        let regex = new RegExp(`(${highlightText})`, 'gi');
        document.body.innerHTML = document.body.innerHTML.replace(regex, `<mark>$1</mark>`);
    }
});


document.addEventListener("DOMContentLoaded", function () {
    // Update Digital Calendar function
    function updateDigitalCalendar() {
        const calendarElement = document.getElementById('digital-calendar');
        const today = new Date();

        // Get the current day, month, and year
        const day = String(today.getDate()).padStart(2, '0');
        const month = String(today.getMonth() + 1).padStart(2, '0');
        const year = today.getFullYear();

        // Set the formatted date in the element
        if (calendarElement) {
            calendarElement.textContent = `${day}/${month}/${year}`;
        }
    }

    // Call the function on page load
    updateDigitalCalendar();

    // Array of quotes
    const quotes = [
        "\"Success is not the key to happiness. Happiness is the key to success.\" - Albert Schweitzer",
        "\"The only way to do great work is to love what you do.\" - Steve Jobs",
        "\"Success is not the key to happiness. Happiness is the key to success.\" - Albert Schweitzer",
        "\"The only way to do great work is to love what you do.\" - Steve Jobs",
        "\"Success is not the key to happiness. Happiness is the key to success.\" - Albert Schweitzer",
        "\"The only way to do great work is to love what you do.\" - Steve Jobs",
        "\"The future belongs to those who believe in the beauty of their dreams.\" - Eleanor Roosevelt",
        "\"In the middle of every difficulty lies opportunity.\" - Albert Einstein",
        "\"Believe you can and you're halfway there.\" - Theodore Roosevelt",
        "\"Don't watch the clock; do what it does. Keep going.\" - Sam Levenson",
        "\"It does not matter how slowly you go as long as you do not stop.\" - Confucius",
        "\"The best way to predict the future is to create it.\" - Peter Drucker",
        "\"Your time is limited, don't waste it living someone else's life.\" - Steve Jobs",
        "\"The only limit to our realization of tomorrow is our doubts of today.\" - Franklin D. Roosevelt",
        "\"Start where you are. Use what you have. Do what you can.\" – Arthur Ashe",
        "\"I’m a great believer in luck, and I find the harder I work, the more I have of it.\" – Thomas Jefferson",
        "\"Challenges are what make life interesting and overcoming them is what makes life meaningful.\" – Joshua J. Marine",
        "\"The only thing that overcomes hard luck is hard work.\" – Harry Golden",
        "\"Alone we can do so little. Together we can do so much.\" – Helen Keller",
        "\"If everyone is moving forward together, then success takes care of itself.\" – Henry Ford",
        "\"How wonderful it is that nobody needs wait a single moment before starting to improve the world.\" — Anne Frank",
        "\"The meeting of preparation with opportunity generates the offspring we call luck.\" – Tony Robbins",
        "\"If you want to go fast, go alone. If you want to go far, go together.\" – African Proverb",
        "\"A diamond is a piece of coal that stuck to the job.\" – Michael Larsen",
        "\"It is amazing how much you can accomplish when it doesn’t matter who gets the credit.\" – Harry S. Truman",
        "\"You can’t use up creativity. The more you use, the more you have.\" – Maya Angelou",
        "\"Your most important work is always ahead of you, never behind you.\" – Stephen Covey",
        "\"A dream doesn’t become reality through magic; it takes sweat, determination and hard work.\" – Colin Powell",
        "\"No great achiever – even those who made it seem easy – ever succeeded without hard work.\" – Jonathan Sack",
        "\"The way to get started is to quit talking and begin doing.\" – Walt Disney Company",
        "\"All growth depends upon activity. There is no development physically or intellectually without effort, and effort means work.\" – Calvin Coolidge",
        "\"There are no shortcuts to any place worth going.\" – Beverly Sills",
        "\"The only place where success comes before work is in the dictionary.\" – Vidal Sassoon",
        "\"We are what we repeatedly do. Excellence, therefore, is not an act but a habit.\" – Aristotle",
        "\"A boat doesn’t go forward if each one is rowing their own way.\" – Proverb",
        "\"If you can’t fly, then run; if you can’t run, then walk; if you can’t walk, then crawl, but whatever you do, you have to keep moving forward.\" – Martin Luther King Jr.",
        "\"I attribute my success to this: I never gave or took any excuse.\" – Florence Nightingale",
        "\"We may encounter many defeats, but we must not be defeated.\" – Maya Angelou",
        "\"If you are working on something that you really care about, you don’t have to be pushed. The vision pulls you.\" – Steve Jobs",
        "\"If your dream is a big dream, and if you want your life to work on the high level that you say you do, there’s no way around doing the work it takes to get you there.\" – Joyce Chapman",
        "\"Limitations live only in our minds. But if we use our imaginations, our possibilities become limitless.\" — Jamie Paolinetti"
    ];

    // Get today's date (get the day of the year)
    const today = new Date();
    const start = new Date(today.getFullYear(), 0, 0);
    const diff = today - start;
    const oneDay = 1000 * 60 * 60 * 24;
    const dayOfYear = Math.floor(diff / oneDay);

    // Select a quote based on the day of the year
    const quoteOfTheDay = quotes[dayOfYear % quotes.length];

    // Display the quote in the element with id "quote"
    const quoteElement = document.getElementById("quote");
    if (quoteElement) {
        quoteElement.innerText = quoteOfTheDay;
    }
});