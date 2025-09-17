<script>
        // Hiệu ứng click cho nút tìm kiếm
    function handleSearch() {
            const button = document.querySelector('.search-button');

    // Tạo hiệu ứng loading cute
    button.innerHTML = '🥰 Đang tìm...';
    button.style.transform = 'scale(0.95)';
            
            setTimeout(() => {
        button.innerHTML = '✨ Hoàn thành! ✨';
    button.style.background = 'linear-gradient(135deg, #ff1493, #ff69b4, #ffb3d9)';

    // Tạo hiệu ứng confetti
    createConfetti();
                
                setTimeout(() => {
        button.innerHTML = '🔍 Tìm kiếm cute';
    button.style.background = 'linear-gradient(135deg, #ff69b4, #ff1493, #ff69b4)';
    button.style.transform = 'scale(1)';
                }, 2000);
            }, 1500);
        }

    // Tạo hiệu ứng confetti
    function createConfetti() {
            const confettiElements = ['🎉', '✨', '🌟', '💖', '🎊'];

    for (let i = 0; i < 20; i++) {
                const confetti = document.createElement('div');
    confetti.innerHTML = confettiElements[Math.floor(Math.random() * confettiElements.length)];
    confetti.style.position = 'fixed';
    confetti.style.left = Math.random() * 100 + 'vw';
    confetti.style.top = '-50px';
    confetti.style.fontSize = Math.random() * 20 + 15 + 'px';
    confetti.style.pointerEvents = 'none';
    confetti.style.zIndex = '1000';
    confetti.style.animation = `confettiFall ${Math.random() * 2 + 2}s ease-out forwards`;

    document.body.appendChild(confetti);
                
                setTimeout(() => {
        confetti.remove();
                }, 4000);
            }
        }

    // CSS cho confetti
    const confettiCSS = `
    @keyframes confettiFall {
        0 % {
            transform: translateY(-50px) rotate(0deg); 
                    opacity: 1;
        }
                100% {
        transform: translateY(100vh) rotate(360deg);
    opacity: 0; 
                }
            }
    `;

    const style = document.createElement('style');
    style.textContent = confettiCSS;
    document.head.appendChild(style);

    // Hiệu ứng cho checkbox và radio
    document.addEventListener('DOMContentLoaded', function() {
            // Hiệu ứng cho checkbox
            const checkboxes = document.querySelectorAll('input[type="checkbox"]');
            checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            if (this.checked) {
                this.closest('.checkbox-container').style.transform = 'scale(1.1)';
                setTimeout(() => {
                    this.closest('.checkbox-container').style.transform = 'scale(1)';
                }, 300);
            }
        });
            });

    // Hiệu ứng cho radio buttons
    const radios = document.querySelectorAll('input[type="radio"]');
            radios.forEach(radio => {
        radio.addEventListener('change', function () {
            const groupName = this.name;
            const allRadiosInGroup = document.querySelectorAll(`input[name="${groupName}"]`);
            allRadiosInGroup.forEach(r => {
                r.closest('.radio-container').style.transform = 'scale(1)';
            });

            this.closest('.radio-container').style.transform = 'scale(1.15)';
            setTimeout(() => {
                this.closest('.radio-container').style.transform = 'scale(1)';
            }, 400);
        });
            });

    // Hiệu ứng cho date input
    const dateInput = document.querySelector('.date-input');
    dateInput.addEventListener('focus', function() {
        this.style.transform = 'translateY(-5px) scale(1.05)';
            });

    dateInput.addEventListener('blur', function() {
        this.style.transform = 'translateY(0) scale(1)';
            });
        });

    // Tạo particles liên tục
    function createParticle() {
            const particle = document.createElement('div');
    particle.className = 'particle';
    particle.style.left = Math.random() * 100 + '%';
    particle.style.width = particle.style.height = Math.random() * 15 + 8 + 'px';
    particle.style.animationDuration = Math.random() * 4 + 6 + 's';
    particle.style.background = `linear-gradient(45deg,
    hsl(${Math.random() * 60 + 300}, 70%, 70%),
    hsl(${Math.random() * 60 + 300}, 80%, 80%))`;

    document.querySelector('.floating-particles').appendChild(particle);
            
            setTimeout(() => {
        particle.remove();
            }, 10000);
        }

    // Tạo particles mới mỗi 1.5 giây
    setInterval(createParticle, 1500);
</script>